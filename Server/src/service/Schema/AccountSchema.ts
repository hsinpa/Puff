import {Schema, model} from 'mongoose';
import {GetDate} from '../../Utility/GeneralMethod';
import {UniversalParameter} from '../../Utility/Flag/EventFlag';

const accountschema = new Schema({
    username : String,
    password : String,
    email : String,
    auth_key : String,
    auth_expire : {type : Date, default : GetDate(UniversalParameter.AuthkeyExpireDate)},
    create_date : {type : Date, default :Date.now},
});

export default accountschema;