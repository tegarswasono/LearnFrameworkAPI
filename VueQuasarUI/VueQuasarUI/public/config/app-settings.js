window.appSettings = {
  openid_connect: {
    authority: 'http://localhost:5290',
    client_id: 'jsapp',
    redirect_uri: 'http://localhost:5173/oidc/callback',
    silent_redirect_uri: 'http://localhost:5173/oidc/silent-renew',
    post_logout_redirect_uri: 'http://localhost:5173/oidc/logout-redirect',
    scope: 'openid roles'
  },
  offsetInMins: 420,
  api: {
    base_url: 'http://localhost:5120'
  }
}
