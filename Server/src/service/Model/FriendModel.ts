import * as moogoose from 'mongoose';
import AccountModel from './AccountModel';
import {FriendStatus, FriendComponentType, DatabaseResultType} from '../../Utility/Flag/TypeFlag';
import {DatabaseErrorType,} from '../../Utility/Flag/EventFlag';

class FriendModel {
    private accountSchema : typeof moogoose.Model;
    private friendSchema : typeof moogoose.Model;
    private accountModel : AccountModel;

    constructor(accountModel : AccountModel, accountSchema : typeof moogoose.Model, friendSchema : typeof moogoose.Model) {
        this.accountModel = accountModel;
        this.accountSchema = accountSchema;
        this.friendSchema = friendSchema;
    }

    async GetFriends(account_id : string) : Promise<any> {
        if (!moogoose.isValidObjectId(account_id))
            return null;

        let r = await this.accountModel.GetFriendInfo(account_id);

        return r;
    }

    async RequestFriend(account_id : string, target_id : string, auth_key : string) : Promise<DatabaseResultType> {
        let targetValid = await this.accountModel.IsAccountExist(target_id);
        let userInfo = await this.accountModel.GetUserInfoWithAuth(account_id, auth_key);
        let relationExist = await this.IsRelationExist(account_id, target_id);

        if (userInfo != null && targetValid && !relationExist) {
            let friendRequest : FriendComponentType = {
                account : account_id,
                recipient : target_id,
                status : FriendStatus.Invite
            }

            let friendReceive : FriendComponentType = {
                account :  target_id,
                recipient : account_id,
                status : FriendStatus.Receive
            }

            let friendRequestResult =  await this.friendSchema.create(friendRequest);
            let friendReceiveResult = await this.friendSchema.create(friendReceive);

            this.accountModel.InsertFriendInfo(account_id, friendRequestResult._id);
            this.accountModel.InsertFriendInfo(target_id, friendReceiveResult._id);

            return { status : DatabaseErrorType.Normal };
        }

        return { status : DatabaseErrorType.Friend.Fail_WhatEverTheReason };
    }

    async AcceptFriend(account_id : string, target_id : string) {
        let r = await this.FindFColumnWithBothID(account_id, target_id);
        let rLength = r.length;

        for(let i = 0; i < rLength; i++) {
            r[i].status = DatabaseErrorType.Normal;
            r[i].save();
        }
    }

    async DenyFriend() {

    }

    private async FindFColumnWithAccountID(account_id : string, target_id : string) {
        return await this.friendSchema.findOne({'account':account_id, 'recipient' : target_id}).
        exec();
    }

    private async FindFColumnWithBothID(account_id : string, target_id : string) {
        return await this.friendSchema.find({
            $or:[ {'account':account_id , 'recipient':target_id},
                  {'account':target_id , 'recipient':account_id}] 
            }).exec();
    }

    private async IsRelationExist(account_id : string, target_id : string) : Promise<boolean> {
        let r = await this.FindFColumnWithBothID(account_id, target_id);

        return r.length > 0;
    }
}

export default FriendModel;