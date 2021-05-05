
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {FriendRouter} from './FriendRouter';
import {AccountRouter} from './AccountRouter';
import {PuffRouter} from './PuffRouter';

module.exports =  (router : Router, mongodb:MongoDB) => {

  router.use(async (ctx : any, next : any) => {
    //I want to check User Authentication here

    await next();
  });

  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('main', {title: "HSINPA"});
  });

  FriendRouter(router, mongodb);
  AccountRouter(router, mongodb);
  PuffRouter(router, mongodb); 
}