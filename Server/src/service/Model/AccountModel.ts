import AccountSchema from '../Schema/AccountSchema';
import * as moogoose from 'mongoose';
import {DatabaseErrorType, UniversalParameter } from '../../Utility/Flag/EventFlag';
import {ClientSignLogType, DatabaseResultType, AccountType } from '../../Utility/Flag/TypeFlag';
import {SHA256Hash, GenerateRandomString, GetDate } from '../../Utility/GeneralMethod';

class AccountModel {

    private accountSchema : typeof moogoose.Model;
    private password_key : string = "akj3#48@!sv";

    constructor(schema : typeof moogoose.Model) {
        this.accountSchema = schema;
    }

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
        let isUnique = await this.IsAccountNoExist(dataset.email);
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

    async IsAccountNoExist(p_email : string) : Promise<boolean>  {
        let r = await this.accountSchema.find({
            email : p_email
        }).
        select({ name: 1 }).
        exec();

        return r.length <= 0;
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

}

export default AccountModel;