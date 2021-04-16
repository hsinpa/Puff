import * as moogoose from 'mongoose';
import PuffSchema from './Schema/PuffSchema';
import AccountSchema from './Schema/AccountSchema';
import FriendSchema from './Schema/FriendSchema';

import AccountModel from './Model/AccountModel';
import PuffModel from './Model/PuffModel';
import FriendModel from './Model/FriendModel';

import {DatabaseTableName} from '../Utility/Flag/EventFlag'

class MongoDB {
    private config = {};
    private dburi : string;
    private moogoseDB : typeof moogoose;

    private puffSchema : typeof moogoose.Model;
    private accountSchema : typeof moogoose.Model;
    private friendSchema : typeof moogoose.Model;

    puffModel : PuffModel;
    accountModel : AccountModel;
    friendModel : FriendModel;

    constructor(env : NodeJS.ProcessEnv, callback :  (db : MongoDB )=> void) {
        this.config = {
            user : env.DATABASE_USER,
            password : env.DATABASE_PASSWORD,
            database : env.DATABASE_NAME,
            host : env.DATABASE_SERVER
        }

        this.dburi = `mongodb+srv://${env.DATABASE_USER}:${env.DATABASE_PASSWORD}@cluster0.hpoyp.mongodb.net/${env.DATABASE_NAME}?retryWrites=true&w=majority`;        
        this.ConnectToDatabase(callback);
    }

    async ConnectToDatabase(callback : (db : MongoDB ) => void) {
        this.moogoseDB = await moogoose.connect(this.dburi, {
            useCreateIndex: true,
            useUnifiedTopology : true, 
            useNewUrlParser : true
        });
        this.RegisterAllSchema();
        this.RegisterAllModel();
        callback(this);
    }

    RegisterAllSchema() {
        this.puffSchema = this.moogoseDB.model(DatabaseTableName.Message, PuffSchema);
        this.accountSchema = this.moogoseDB.model(DatabaseTableName.Account, AccountSchema);
        this.friendSchema = this.moogoseDB.model(DatabaseTableName.Friend, FriendSchema);
    }

    RegisterAllModel() {
        this.accountModel = new AccountModel(this.accountSchema);
        this.puffModel = new PuffModel(this.puffSchema, this.accountSchema);
        this.friendModel = new FriendModel(this.accountModel, this.accountSchema, this.friendSchema);
    }

}

export default MongoDB;