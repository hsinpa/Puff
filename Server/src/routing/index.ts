
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {PuffMessageType, ClientSignLogType} from '../Utility/Flag/TypeFlag';
import bodyParser = require('koa-bodyparser');

module.exports =  (router : Router, mongodb:MongoDB) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

//#region Account Management 
  router.post('/login', async function (ctx:any, next:any) {
    let r = JSON.stringify(await mongodb.accountModel.Login(ctx.request.body as ClientSignLogType));
    console.log(r);

    ctx.body = r;
  });

  router.post('/auth_login', async function (ctx:any, next:any) {
    let r = JSON.stringify(await mongodb.accountModel.LoginWithAuthkey(ctx.request.body.account_id, 
                                                                      ctx.request.body.auth_key));
    console.log(r);

    ctx.body = r;
  });

  router.post('/sign_up', async function (ctx:any, next:any) {
    let r = JSON.stringify(await mongodb.accountModel.SignUp(ctx.request.body as ClientSignLogType));
    console.log(r);
    ctx.body = r;
  });
//#endregion
  router.get('/get_all/:latitude/:longtitude/:radius', async function (ctx:any, next:any) {
    //ctx.params.latitude
    //ctx.params.longtitude
    //ctx.params.radius

    let r = JSON.stringify(await mongodb.puffModel.GetAllPuff());

    ctx.body = r;
  });

  router.post('/send_puff_comment', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.SavePuffComment(ctx.request.body.message_id, ctx.request.body.author_id, ctx.request.body.author, ctx.request.body.body );

    ctx.body = r;
  });

  router.post('/send_puff_msg', async function (ctx:any, next:any) {
    let msgType : PuffMessageType = {
      author : ctx.request.body['author'],
      author_id : ctx.request.body['author_id'],
      body : ctx.request.body['body'],
      comments : []
    }

    let result = await mongodb.puffModel.SavePuffRecord(msgType);

    ctx.body = result;
  });


}