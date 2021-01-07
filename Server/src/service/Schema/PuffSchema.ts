import {Schema, model} from 'mongoose';

const commentSchema = new Schema({
    author_id : String,
    author : String,
    body : String,
    message_id : String,
    date : {type : Date, default :Date.now}
});

const puffSchema = new Schema({
    author_id : String,
    author : String,
    body : String,
    comments : [commentSchema],
    date : {type : Date, default :Date.now},
    expire : {type:Date, default :Date.now}
});

export default puffSchema;