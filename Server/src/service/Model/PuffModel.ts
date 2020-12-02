import PuffSchema from '../Schema/PuffSchema';
import * as moogoose from 'mongoose';
import {PuffCommentType, PuffMessageType } from '../../Utility/Flag/TypeFlag'

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
        let result = await this.puffSchema.create(puffMsg );
    }

    async SavePuffComment(puff_id : string, ) {

    }

}

export default PuffModel;