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
    privacy : Number, //0 = public, 1 = friend, 2 = anonymous
    // latitude : Number,
    // longitude : Number,
    geo_location : {
        type: { type: String },
        coordinates: [Number]
    },
    images : [String],
    comments : [commentSchema],
    date : {type : Date, default :Date.now},
    expire : {type: Date}
}).index({ geo_location: "2dsphere" });

export default puffSchema;