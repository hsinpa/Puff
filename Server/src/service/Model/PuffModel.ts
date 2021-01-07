import PuffSchema from '../Schema/PuffSchema';
import * as moogoose from 'mongoose';
import {PuffCommentType, PuffMessageType } from '../../Utility/Flag/TypeFlag';
import * as uuid from 'uuid';

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
        return await this.puffSchema.create(puffMsg );
    }

    async SavePuffComment(puff_id : string, author_id : string, author : string, body : string ) {
        // let comment_uuid = uuid.v4();
        
        let puffObject = await this.puffSchema.findById(puff_id);
        puffObject.comments.push( {author_id : author_id, author: author, body : body, message_id : puff_id});
        return await puffObject.save();
    }

}

export default PuffModel;