import * as moogoose from 'mongoose';
import {DatabaseTableName} from '../../../Utility/Flag/EventFlag';

export function FriendQuery(accountSchema : typeof moogoose.Model, account_id : string) {
    return accountSchema.aggregate([
        {$match : {_id : moogoose.Types.ObjectId(account_id)}},
        {$lookup: {
            from : DatabaseTableName.Friend,
            localField : "friends",
            foreignField : "_id",
            as : "friend_table"
        }},
        {$lookup : {
            from : DatabaseTableName.Account,
            localField : "friend_table.recipient",
            foreignField : "_id",
            as : "friend_info"
        }} ,
        {$project : {email : 1, friend_info : {

            $map: { 
                'input': '$friend_info', 
                'as': 'friend_info', 
                'in': {
                    '_id' : "$$friend_info._id",
                    'email' : "$$friend_info.email",
                    'username': '$$friend_info.username',
                    'status': {
                        "$arrayElemAt": [
                          "$friend_table.status",
                          { "$indexOfArray": [ "$friend_table.recipient", "$$friend_info._id" ] }
                        ]
                      },
                }
            }
        }}}
    ]);
}