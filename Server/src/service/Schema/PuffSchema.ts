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
    title : String,
    type: Number, //0 = floating seed, 1 = plant
    duration : Number,
    privacy : Number,
    latitude : Number,
    longitude : Number,
    images : [String],
    comments : [commentSchema],
    date : {type : Date, default :Date.now},
    expire : {type: Date}
});

export default puffSchema;