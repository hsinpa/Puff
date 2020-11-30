
import * as path from 'path';
import * as Router from 'koa-router';

module.exports =  (router : Router, rootPath:string) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });
}
