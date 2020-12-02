import * as moogoose from 'mongoose';
import PuffSchema from './Schema/PuffSchema';
import PuffModel from './Model/PuffModel';

class MongoDB {
    private config = {};
    private dburi : string;
    private moogoseDB : typeof moogoose;

    private puffSchema : typeof moogoose.Model;
    
    puffModel : PuffModel;

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
        this.moogoseDB = await moogoose.connect(this.dburi, {useUnifiedTopology : true, useNewUrlParser : true});
        this.RegisterAllSchema();
        this.RegisterAllModel();
        callback(this);
    }

    RegisterAllSchema() {
        this.puffSchema = this.moogoseDB.model("puff_record", PuffSchema);
        this.puffSchema.schema
    }

    RegisterAllModel() {
        this.puffModel = new PuffModel(this.puffSchema);
    }

}

export default MongoDB;