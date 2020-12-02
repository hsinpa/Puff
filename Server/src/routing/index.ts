
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {PuffMessageType} from '../Utility/Flag/TypeFlag';
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

  router.get('/test_save_puff/:author_id/:author/:body', async function (ctx:any, next:any) {
    //let r = ctx.params.author_id +"/"+ ctx.params.author + "/"+ctx.params.body;

    let r : PuffMessageType = {
      author : ctx.params.author,
      author_id : ctx.params.author_id,
      body : ctx.params.body
    }

    await mongodb.puffModel.SavePuffRecord(r);

    ctx.body = r;
  });

}