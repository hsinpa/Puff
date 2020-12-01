
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';

module.exports =  (router : Router, mongodb:MongoDB) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

  router.get('/get_all', async function (ctx:any, next:any) {
    let r = await mongodb.puffSchema.find();
    console.log(r);

    ctx.body = r;
  });
}