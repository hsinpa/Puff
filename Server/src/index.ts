import * as http from 'http';
import * as koa_static from 'koa-static';
import * as Router from 'koa-router';

const Koa = require('koa');
const bodyParser = require('koa-bodyparser');
const path = require('path')
const rootRouter = require('./routing');
const so = require('koa-views');

const router = new Router();
const views = require('koa-views');

const dotenv = require('dotenv');
dotenv.config();

const env = process.env;
const app = new Koa();

let rootFolder : string = path.join(__dirname, '..',);

app.use(koa_static(
  path.join( rootFolder,  '/views')
));

app.use(views(rootFolder + '/views', {
  map: {
    html: 'handlebars'
  }
}));

app.use(bodyParser());
app.use(router.routes())
app.use(router.allowedMethods())

const dburi = `mongodb+srv://hsinpa:${env.DATABASE_PASSWORD}@cluster0.hpoyp.mongodb.net/${env.DATABASE_NAME}?retryWrites=true&w=majority`;

// @ts-ignore
var server = http.Server(app.callback());

rootRouter(router, rootFolder);

//"192.168.0.86"
server.listen(env.NODE_PORT || 8020, 'localhost', function () {
  console.log(`Application worker ${process.pid} started...`);
});