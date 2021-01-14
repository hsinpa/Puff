import PuffSchema from '../Schema/PuffSchema';
import * as moogoose from 'mongoose';
import {PuffCommentType, PuffMessageType, Duration } from '../../Utility/Flag/TypeFlag';
import * as uuid from 'uuid';
import {GetDate } from '../../Utility/GeneralMethod';


class PuffModel {

    private puffSchema : typeof moogoose.Model;

    constructor(schema : typeof moogoose.Model) {
        this.puffSchema = schema;
    }

    async GetAllPuff() {
        let r = await this.puffSchema.find();
        return r;
    }

    async SavePuffRecord(puffMsg : PuffMessageType) {
        puffMsg.date = new Date(Date.now());
        puffMsg.expire = this.AddNumberToDate(puffMsg.date, puffMsg.duration);
        
        return await this.puffSchema.create(puffMsg );
    }

    private AddNumberToDate(date : Date, appendDate : number) : Date {
        let expireDate = new Date();
        switch(appendDate as Duration) {
            case Duration.Day :
                expireDate = GetDate(1);
                break;

            case Duration.Week :
                expireDate = GetDate(7);
                break;

            case Duration.Month :
                expireDate = GetDate(30);
                break;
        }

        return (expireDate);
    }

    async SavePuffComment(puff_id : string, author_id : string, author : string, body : string ) {
        // let comment_uuid = uuid.v4();
        let puffObject = await this.puffSchema.findById(puff_id);
        puffObject.comments.push( {author_id : author_id, author: author, body : body, message_id : puff_id});
        return await puffObject.save();
    }

}

export default PuffModel;