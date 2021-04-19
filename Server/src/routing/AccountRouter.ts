
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {ClientSignLogType, DatabaseResultType} from '../Utility/Flag/TypeFlag';
import {DatabaseErrorType } from '../Utility/Flag/EventFlag';

export let AccountRouter = function (router : Router, mongodb:MongoDB) {

    router.get('/account/publicInfo/email/:email', async function (ctx:any, next:any) {
      let r = (await mongodb.accountModel.GetPublicAccountByEmail(ctx.params.email));

      let returnType  : DatabaseResultType = {
        status : (r.length > 0) ? DatabaseErrorType.Normal : DatabaseErrorType.Account.Fail_Login_NoAccount,
        result :  JSON.stringify((r.length > 0) ? r[0] : {})
      };

      ctx.body = JSON.stringify(returnType);
    });

    router.get('/account/publicInfo/id/:id', async function (ctx:any, next:any) {
      let r = (await mongodb.accountModel.GetPublicAccountByID(ctx.params.id));

      let returnType  : DatabaseResultType = {
        status : (r != null) ? DatabaseErrorType.Normal : DatabaseErrorType.Account.Fail_Login_NoAccount,
        result :  JSON.stringify((r != null) ? r : {})
      };

      ctx.body = JSON.stringify(returnType);
    });

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