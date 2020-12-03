
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {PuffMessageType, PuffCommentType} from '../Utility/Flag/TypeFlag';
import bodyParser = require('koa-bodyparser');

module.exports =  (router : Router, mongodb:MongoDB) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

  router.get('/get_all', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.GetAllPuff();
    console.log(r);

    ctx.body = r;
  });

  router.post('/send_puff_comment', async function (ctx:any, next:any) {
    let r = await mongodb.puffModel.SavePuffComment(ctx.request.body.puff_id, ctx.request.body.author_id, ctx.request.body.author, ctx.request.body.body );

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