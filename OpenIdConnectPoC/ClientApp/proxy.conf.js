const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:61206';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/auth",
      "/signin",
      "/values",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    },
    cookieDomainRewrite: "localhost"
  }
]

module.exports = PROXY_CONFIG;
