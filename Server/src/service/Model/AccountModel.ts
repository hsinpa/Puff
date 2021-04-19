import AccountSchema from '../Schema/AccountSchema';
import * as moogoose from 'mongoose';
import {DatabaseErrorType, UniversalParameter } from '../../Utility/Flag/EventFlag';
import {ClientSignLogType, DatabaseResultType, AccountType } from '../../Utility/Flag/TypeFlag';
import {SHA256Hash, GenerateRandomString, GetDate } from '../../Utility/GeneralMethod';
import {FriendQuery} from './Query/AccountQuery';

class AccountModel {

    private accountSchema : typeof moogoose.Model;
    private password_key : string = "akj3#48@!sv";

    constructor(schema : typeof moogoose.Model) {
        this.accountSchema = schema;
    }

//#region Utility Functions
async GetPublicAccountByEmail(p_email : string) {
    return await this.accountSchema.find({
        email : p_email
    }).
    select("username _id email").
    exec();
}

async GetPublicAccountByID(id : string) {
    return await this.accountSchema.findById(id).
    select("username _id email").
    exec();
}

async IsEmailNoExist(p_email : string) : Promise<boolean>  {
    let r = await this.GetPublicAccountByEmail(p_email);

    return r.length <= 0;
}

async IsAccountExist(account_id : string) : Promise<boolean>  {
    let r = await this.accountSchema.findById(account_id).exec();

    return r != null;
}

async GetUserInfo(p_email : string, p_password : string) {
    let hashPassword = SHA256Hash(p_password + this.password_key);
    let r = await this.accountSchema.findOne({
        email : p_email,
        password : hashPassword
    }).
    select("username email _id auth_key auth_expire").
    exec();

    return r;
}

async IsUserValidWithAuth(p_id : string, p_auth : string) {
    return (await this.GetUserInfoWithAuth(p_id, p_auth)) != null;
}

async GetUserInfoWithAuth(p_id : string, p_auth : string) {
    if (!moogoose.isValidObjectId(p_id))
        return null;

    let r = await this.accountSchema.findOne({
        _id : p_id,
        auth_key : p_auth
    }).
    select("username email _id auth_key auth_expire").
    exec();

    return r;
}
//#endregion

//#region Login SignUp
    async LoginWithAuthkey(account_id : string, auth_key : string) {
        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Account.Fail_AuthLogin_NotValid,
            result : {}
        };

        console.log(`account id ${account_id}, auth_key ${auth_key}`);

        let userInfo = await this.GetUserInfoWithAuth(account_id, auth_key);
        //Find none
        if (userInfo == null) {
            console.log("AuthLogin Not valid");
            return returnType;
        }

        let expireDate = new Date(userInfo.auth_expire);
        let currentTime = new Date(Date.now());

        //Is expire
        if (expireDate < currentTime)
            return returnType;

        userInfo.auth_expire = GetDate(UniversalParameter.AuthkeyExpireDate);
        await userInfo.save();

        returnType.status = DatabaseErrorType.Normal;
        returnType.result = JSON.stringify(userInfo);

        return returnType;
    }

    async Login(dataset : ClientSignLogType) {
        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Normal,
            result : {}
        };

        let userInfo = await this.GetUserInfo(dataset.email, dataset.password);
        
        if (userInfo == null)
            returnType.status = DatabaseErrorType.Account.Fail_Login_NoAccount;
        else {
            console.log("Pre authkey " + userInfo.auth_key);
            userInfo.auth_key = GenerateRandomString(10);
            userInfo.auth_expire = GetDate(UniversalParameter.AuthkeyExpireDate);
            await userInfo.save();

            console.log("After authkey " + userInfo.auth_key);
            returnType.result = JSON.stringify(userInfo);
        }

        return returnType;
    }

    async SignUp(dataset : ClientSignLogType) {
        let isUnique = await this.IsEmailNoExist(dataset.email);
        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Normal,
            result : {}
        };

        if (!isUnique)
            returnType.status = DatabaseErrorType.Account.Fail_SignUp_DuplicateAccount;
        else {
            dataset.password = SHA256Hash(dataset.password + this.password_key);
            dataset.auth_key = GenerateRandomString(10);
            let r = await this.accountSchema.create(dataset);
            r.password = undefined;

            returnType.result =JSON.stringify(r);
        }

        return returnType;
    }
//#endregion

//#region Friend Relate
    async GetFriendInfo(p_id : string) {
        return await FriendQuery(this.accountSchema, p_id).exec();
    }

    async InsertFriendInfo(p_id : string, relation_id : string) {
        let puffObject = await this.accountSchema.findById(p_id);

        puffObject.friends.push(relation_id);
        return await puffObject.save();
    }

    async RemoveFriend(p_id : string, relation_id : string) {
        let puffObject = await this.accountSchema.findById(p_id);
        let ObjectRID =  moogoose.Types.ObjectId(relation_id);

        puffObject.friends.pull(ObjectRID);
        return await puffObject.save();
    }
//#endregion

}

export default AccountModel;