import * as moogoose from 'mongoose';

class FriendModel {
    private accountSchema : typeof moogoose.Model;
    private friendSchema : typeof moogoose.Model;

    constructor(accountSchema : typeof moogoose.Model, friendSchema : typeof moogoose.Model) {
        this.accountSchema = accountSchema;
        this.friendSchema = friendSchema;
    }

    async GetFriends(account_id : string) : Promise<any> {
        if (!moogoose.isValidObjectId(account_id))
            return null;

        let r = await this.friendSchema.find({
            $or:[ {'requester':account_id}, {'recipient':account_id}] }).
        exec();

        console.log(r);
    }

    AcceptFriend() {

    }

    RequestFriend() {

    }

    DenyFriend() {

    }
}

export default FriendModel;