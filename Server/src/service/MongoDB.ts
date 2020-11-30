import * as moogoose from 'mongoose';

class MongoDB {
    
    config = {};
    dburi : string;

    constructor(env : NodeJS.ProcessEnv) {
        this.config = {
            user : env.DATABASE_USER,
            password : env.DATABASE_PASSWORD,
            database : env.DATABASE_NAME,
            host : env.DATABASE_SERVER
        }

        this.dburi = `mongodb+srv://${env.DATABASE_USER}:${env.DATABASE_PASSWORD}@cluster0.hpoyp.mongodb.net/${env.DATABASE_NAME}?retryWrites=true&w=majority`;
        moogoose.connect(this.dburi, {useUnifiedTopology : true, useNewUrlParser : true});
    }
}

export default MongoDB;