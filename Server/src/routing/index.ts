
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {FriendRouter} from './FriendRouter';
import {AccountRouter} from './AccountRouter';

import {PuffMessageType, ClientSignLogType} from '../Utility/Flag/TypeFlag';
import bodyParser = require('koa-bodyparser');

module.exports =  (router : Router, mongodb:MongoDB) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

//#region Puff Message 
router.get('/get_all/:latitude/:longtitude/:radius', async function (ctx:any, next:any) {
  //ctx.params.latitude
  //ctx.params.longtitude
  //ctx.params.radius

  let r = JSON.stringify(await mongodb.puffModel.GetAllPuff());

  ctx.body = r;
});

router.get('/get_all', async function (ctx:any, next:any) {
  let r = JSON.stringify(await mongodb.puffModel.GetAllPuff());
  ctx.body = r;
});

router.post('/send_puff_comment', async function (ctx:any, next:any) {
  let r = await mongodb.puffModel.SavePuffComment(ctx.request.body.message_id, ctx.request.body.author_id, ctx.request.body.author, ctx.request.body.body );

  ctx.body = r;
});

router.post('/send_puff_msg', async function (ctx:any, next:any) {
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

  FriendRouter(router, mongodb);
  AccountRouter(router, mongodb);
}