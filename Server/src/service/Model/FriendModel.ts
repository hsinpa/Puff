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

        let r = await this.FindFColumnWithAccountID(account_id);

        console.log(r);
    }

    async RequestFriend(account_id : string, target_id : string, auth_key : string) : Promise<DatabaseResultType> {
        let targetValid = !await this.accountModel.IsAccountNoExist(target_id);
        let userInfo = await this.accountModel.GetUserInfoWithAuth(account_id, auth_key);
        let relationExist = await this.IsRelationExist(account_id, target_id);

        if (userInfo != null && targetValid && !relationExist) {
            let status = FriendStatus.Pending;
            let friendRequest : FriendComponentType = {
                requester : account_id,
                recipient : target_id,
                status : status
            }

            this.friendSchema.create(friendRequest);
            
            return { status : DatabaseErrorType.Normal };
        }

        return { status : DatabaseErrorType.Friend.Fail_WhatEverTheReason };
    }

    AcceptFriend() {

    }

    DenyFriend() {

    }

    private async FindFColumnWithAccountID(account_id : string) {
        return await this.friendSchema.find({
            $or:[ {'requester':account_id}, {'recipient':account_id}] }).
        exec();
    }

    private async FindFColumnWithBothID(account_id : string, target_id : string) {
        return await this.friendSchema.findOne({
            $or:[ {'requester':account_id , 'recipient':target_id},
                  {'requester':target_id , 'recipient':account_id}] 
            }).exec();
    }

    private async IsRelationExist(account_id : string, target_id : string) : Promise<boolean> {
        let r = await this.FindFColumnWithBothID(account_id, target_id);

        return r != null;
    }

}

export default FriendModel;