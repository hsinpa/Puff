import {Schema, model} from 'mongoose';

const accountschema = new Schema({
    username : String,
    password : String,
    email : String,
    create_date : {type : Date, default :Date.now},
});

export default accountschema;