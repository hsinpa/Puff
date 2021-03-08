import * as moogoose from 'mongoose';
import AccountModel from './AccountModel';
import {FriendStatus, FriendComponentType, DatabaseResultType} from '../../Utility/Flag/TypeFlag';
import {DatabaseErrorType,} from '../../Utility/Flag/EventFlag';

class FriendModel {
    private accountSchema : typeof moogoose.Model;
    private friendSchema : typeof moogoose.Model;
    private accountModel : AccountModel;

    private errorMessage : any;

    constructor(accountModel : AccountModel, accountSchema : typeof moogoose.Model, friendSchema : typeof moogoose.Model) {
        this.accountModel = accountModel;
        this.accountSchema = accountSchema;
        this.friendSchema = friendSchema;
        this.errorMessage = {status : DatabaseErrorType.Friend.Fail_WhatEverTheReason};
    }

    async GetFriends(account_id : string) : Promise<any> {
        if (!moogoose.isValidObjectId(account_id))
            return this.errorMessage;

        let r = await this.accountModel.GetFriendInfo(account_id);

        if (r.length > 0)
            return r[0];

        return this.errorMessage;
    }

    async RequestFriend(account_id : string, target_id : string, auth_key : string) : Promise<DatabaseResultType> {
        let targetValid = await this.accountModel.IsAccountExist(target_id);
        let isUserAuthValid = await this.accountModel.IsUserValidWithAuth(account_id, auth_key);
        let relationExist = await this.IsRelationExist(account_id, target_id);

        if (isUserAuthValid != null && targetValid && !relationExist) {
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

    async AcceptFriend(account_id : string, target_id : string, auth_key : string) : Promise<DatabaseResultType> {
        let isUserAuthValid = await this.accountModel.IsUserValidWithAuth(account_id, auth_key);

        if (!isUserAuthValid)
            return { status : DatabaseErrorType.Friend.Fail_WhatEverTheReason };

        let r = await this.FindFColumnWithBothID(account_id, target_id);
        let rLength = r.length;

        for(let i = 0; i < rLength; i++) {
            r[i].status = DatabaseErrorType.Normal;
            r[i].save();
        }
        return { status : DatabaseErrorType.Normal };
    }

    async RejectFriend(account_id : string, target_id : string, auth_key : string) {
        let isUserAuthValid = await this.accountModel.IsUserValidWithAuth(account_id, auth_key);

        if (!isUserAuthValid)
            return { status : DatabaseErrorType.Friend.Fail_WhatEverTheReason };
        
        let r = await this.FindFColumnWithBothID(account_id, target_id);
        let rLength = r.length;
    
        //Remove friend from account array
        for(let i = 0; i < rLength; i++) {
            this.accountModel.RemoveFriend(r[i].account, r[i]._id);
            this.accountModel.RemoveFriend(r[i].recipient, r[i]._id);
        }

        this.friendSchema.deleteMany({
            $or:[ {'account':account_id , 'recipient':target_id},
            {'account':target_id , 'recipient':account_id}] 
        }).exec();

        return { status : DatabaseErrorType.Normal };
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