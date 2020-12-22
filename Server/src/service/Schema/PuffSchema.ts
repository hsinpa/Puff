import {Schema, model} from 'mongoose';

const commentSchema = new Schema({
    _id : String,
    author_id : String,
    author : String,
    body : String,
    date : {type : Date, default :Date.now}
});

const puffSchema = new Schema({
    _id : String,
    author_id : String,
    author : String,
    body : String,
    comments : [commentSchema],
    date : {type : Date, default :Date.now},
    expire : {type:Date, default :Date.now}
});

export default puffSchema;