
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {PuffMessageType} from '../Utility/Flag/TypeFlag';

export let PuffRouter = function (router : Router, mongodb:MongoDB) {

  //#region  Send / Save messages
  router.get('/puff/get_all/:latitude/:longtitude/:radius', async function (ctx:any, next:any) {
    //ctx.params.latitude
    //ctx.params.longtitude
    //ctx.params.radius
  
    let r = JSON.stringify(await mongodb.puffModel.GetAllPuff());
  
    ctx.body = r;
  });
  
  router.get('/puff/get_all', async function (ctx:any, next:any) {
    let r = JSON.stringify(await mongodb.puffModel.GetAllPuff());
    ctx.body = r;
  });
  
  router.post('/puff/send_puff_comment', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.SavePuffComment(ctx.request.body.message_id, ctx.request.body.author_id, ctx.request.body.author, ctx.request.body.body );
  
    ctx.body = r;
  });
  
  router.post('/puff/send_puff_msg', async function (ctx:any, next:any) {
    // let msgType : PuffMessageType = {
    //   author : ctx.request.body['author'],
    //   author_id : ctx.request.body['author_id'],
    //   body : ctx.request.body['body'],
    //   comments : []
    // }
  
    delete ctx.request.body['_id'];
    let msgType : PuffMessageType = ctx.request.body;
  
    let result = await mongodb.puffModel.SavePuffRecord(msgType);
  
    ctx.body = result;
  });
//#endregion

//#region Save Messages to personal library
  router.post('/puff/get_all_self_msg', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.GetAllSelfSavePuffMsg(ctx.request.body.account_id);

    ctx.body = r;
  });

  router.post('/puff/save_puff_to_library', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.SavePuffMsgToLibrary(ctx.request.body.puff_id, ctx.request.body.account_id);
  
    ctx.body = r;
  });

  router.post('/puff/remove_puff_from_library', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.RemovePuffMsgFromLibrary(ctx.request.body.puff_id, ctx.request.body.account_id);
  
    ctx.body = r;
  });
//#endregion
}