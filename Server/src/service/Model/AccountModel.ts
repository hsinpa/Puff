import AccountSchema from '../Schema/AccountSchema';
import * as moogoose from 'mongoose';
import {DatabaseErrorType } from '../../Utility/Flag/EventFlag';
import {ClientSignLogType, DatabaseResultType, AccountType } from '../../Utility/Flag/TypeFlag';
import {SHA256Hash} from '../../Utility/GeneralMethod';

class AccountModel {

    private accountSchema : typeof moogoose.Model;
    private password_key : string = "akj3#48@!sv";

    constructor(schema : typeof moogoose.Model) {
        this.accountSchema = schema;
    }

    async Login(dataset : ClientSignLogType) {
        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Normal,
            result : {}
        };

        let userInfoArray = await this.GetUserInfo(dataset.email, dataset.password);
        
        if (userInfoArray.length <= 0)
            returnType.status = DatabaseErrorType.Account.Fail_Login_NoAccount;
        else {
            returnType.result =JSON.stringify(userInfoArray[0]);
        }

        return returnType;
    }

    async SignUp(dataset : ClientSignLogType) {
        let isUnique = await this.IsAccountExist(dataset.email);
        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Normal,
            result : {}
        };

        if (!isUnique)
            returnType.status = DatabaseErrorType.Account.Fail_SignUp_DuplicateAccount;
        else {
            dataset.password = SHA256Hash(dataset.password + this.password_key);
            let r = await this.accountSchema.create(dataset);
            r.password = undefined;

            returnType.result =JSON.stringify(r);
        }

        return returnType;
    }

    async IsAccountExist(p_email : string) : Promise<boolean>  {
        let r = await this.accountSchema.find({
            email : p_email
        }).
        select({ name: 1 }).
        exec();

        return r.length <= 0;
    }

    async GetUserInfo(p_email : string, p_password : string) {
        let hashPassword = SHA256Hash(p_password + this.password_key);

        let r = await this.accountSchema.find({
            email : p_email,
            password : hashPassword
        }).
        select("username email _id").
        exec();

        return r;
    }

}

export default AccountModel;