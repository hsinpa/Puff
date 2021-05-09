import PuffSchema from '../Schema/PuffSchema';
import * as moogoose from 'mongoose';
import {PuffCommentType, PuffMessageType, Duration, DatabaseResultType } from '../../Utility/Flag/TypeFlag';
import {AccountSchemeTableKey, DatabaseErrorType, UniversalParameter} from '../../Utility/Flag/EventFlag';

import * as uuid from 'uuid';
import {GetDate } from '../../Utility/GeneralMethod';
import accountschema from '../Schema/AccountSchema';
import {FilterPuffQuery} from './Query/AccountQuery';

class PuffModel {

    private puffSchema : typeof moogoose.Model;
    private accountSchema : typeof moogoose.Model;

    constructor(puffSchema : typeof moogoose.Model, accountSchema : typeof moogoose.Model ) {
        this.puffSchema = puffSchema;
        this.accountSchema = accountSchema;
    }
    async GetAllPuff() {
        let r = await this.puffSchema.find();
        return r;
    }

    //API not for production yet
    async GetFilteredPuff(account_id : string, latitude : number, longitude : number, range : number) {
        let r = (await FilterPuffQuery(this.accountSchema, account_id))[0];
        let friendList : string[] = r["friend_list"];
        let rangeToMile = range / UniversalParameter.KMPerEquatorialRadius
        let plantRange = 0.1 / UniversalParameter.KMPerEquatorialRadius; //100 meters

        let finalQuery = await this.puffSchema.find(
            {
                $and: [
                    {$or: [
                        { _id: { $in: friendList } }, //If the owner is user's friend
                        { $or: [ { privacy: 0}, { privacy: 2 }] }  //If privacy setting is Public, Anonymous
                    ]},
                    {
                        $or: [
                            //If the message is within range
                            { type : 0, geo_location: { $geoWithin: { $centerSphere: [   [ longitude,latitude ] ,  rangeToMile]} } },
                            //If the message is plant type
                            { type : 1, geo_location: { $geoWithin: { $centerSphere: [  [ longitude, latitude ] ,  plantRange ]} } },
                            //Demo only, 3d avatar man, talk to user
                            { type : 2 }
                        ]              
                    }
                ]
            }
            ).limit(20);
        
        return JSON.stringify(finalQuery);
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

    async FindPuffWithIDs(puffIDList : string[]) {
        let puffObject = await this.puffSchema.find({ _id : { $in : puffIDList } });
        
        return puffObject;
    }

    async GetAllSelfSavePuffMsg(account_id : string) {
        let savePuffs = (await this.GetPuffIDsByAccountID(account_id))[AccountSchemeTableKey.SavePuffMsgList];
        let selfPuffs = await this.GetSelfWritePuffIDs(account_id);
        let selfPuffIDs : string[] = selfPuffs.map(x=> x._id).concat(savePuffs);

        return this.FindPuffWithIDs(selfPuffIDs)
    }

    private async GetPuffIDsByAccountID(account_id : string) {
        return await this.accountSchema.findById(account_id).select(`${AccountSchemeTableKey.SavePuffMsgList}`);
    }

    private async GetSelfWritePuffIDs(account_id : string) {
        return await this.puffSchema.find({author_id : account_id}).select("_id");
    }

    //#region To Personal Library
    async SavePuffMsgToLibrary(puff_id : string, user_id : string) : Promise<DatabaseResultType> {
        //Check if no duplicate
        let msgs = await this.GetPuffIDsByAccountID(user_id);
        let puffList : string[] = msgs[AccountSchemeTableKey.SavePuffMsgList];

        let returnType  : DatabaseResultType = {
            status : DatabaseErrorType.Account.Fail_AuthLogin_NotValid
        };

        let isNoDuplicate = puffList.findIndex(x=> x == puff_id) < 0;

        if (isNoDuplicate) {
            msgs[AccountSchemeTableKey.SavePuffMsgList].push(puff_id);
            await msgs.save();
            returnType.status = DatabaseErrorType.Normal;
            return returnType;
        }

        return returnType;
    }

    async RemovePuffMsgFromLibrary(puff_id : string, user_id : string) : Promise<boolean> {
        let msgs = await this.GetPuffIDsByAccountID(user_id);
        let puffList : string[] = msgs[AccountSchemeTableKey.SavePuffMsgList];

        let puffIndex = puffList.findIndex(x=> x == puff_id);

        if (puffIndex >= 0) {

            msgs[AccountSchemeTableKey.SavePuffMsgList].splice(puffIndex, 1);

            await msgs.save();

            return true;
        }

        return false;
    }
    //#endregion
}

export default PuffModel;