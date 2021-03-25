
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {ClientSignLogType} from '../Utility/Flag/TypeFlag';

export let AccountRouter = function (router : Router, mongodb:MongoDB) {
    router.post('/account/login', async function (ctx:any, next:any) {
        let r = JSON.stringify(await mongodb.accountModel.Login(ctx.request.body as ClientSignLogType));
        console.log(r);
    
        ctx.body = r;
      });
    
      router.post('/account/auth_login', async function (ctx:any, next:any) {
        let r = JSON.stringify(await mongodb.accountModel.LoginWithAuthkey(ctx.request.body.account_id, ctx.request.body.auth_key));
    
        ctx.body = r;
      });
    
      router.post('/account/sign_up', async function (ctx:any, next:any) {
        let r = JSON.stringify(await mongodb.accountModel.SignUp(ctx.request.body as ClientSignLogType));
        console.log(r);
        ctx.body = r;
      });
}