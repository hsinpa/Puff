import AccountSchema from '../Schema/AccountSchema';
import * as moogoose from 'mongoose';
import {PuffCommentType, PuffMessageType } from '../../Utility/Flag/TypeFlag';
import * as uuid from 'uuid';

class AccountModel {

    private accountSchema : typeof moogoose.Model;

    constructor(schema : typeof moogoose.Model) {
        this.accountSchema = schema;
    }

}

export default AccountModel;