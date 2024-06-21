import axios from 'axios'
import { UserManager, WebStorageStateStore, User } from 'oidc-client'

export default class AuthService {
  static instance: AuthService
  private userManager: UserManager

  private constructor() {
    const settings: any = {
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority: (<any>window).appSettings.openid_connect.authority,
      client_id: (<any>window).appSettings.openid_connect.client_id,
      redirect_uri: (<any>window).appSettings.openid_connect.redirect_uri,
      automaticSilentRenew: true,
      silent_redirect_uri: (<any>window).appSettings.openid_connect.silent_redirect_uri,
      response_type: 'code',
      scope: (<any>window).appSettings.openid_connect.scope,
      post_logout_redirect_uri: (<any>window).appSettings.openid_connect.post_logout_redirect_uri,
      filterProtocolClaims: true
    }

    this.userManager = new UserManager(settings)
  }

  public static getInstance(): AuthService {
    if (!AuthService.instance) {
      AuthService.instance = new AuthService()
    }

    return AuthService.instance
  }

  public getIdsUrl(): string {
    return String(this.userManager.settings.authority)
  }

  public getSocketUrl(): string {
    return (<any>window).appSettings.socketUrl
  }

  // public getUser(): Promise<User> {
  //   return this.userManager.getUser()
  // }

  public async renewToken(): Promise<boolean> {
    try {
      const model = {
        grant_type: 'refresh_token',
        refresh_token: localStorage.getItem('refresh_token') || ''
      }

      const headers = {
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      }

      const response = await axios.post(
        `${this.userManager.settings.authority}/connect/token`,
        model,
        headers
      )

      if (response.status === 200) {
        localStorage.setItem('access_token', response.data.access_token)
        localStorage.setItem('refresh_token', response.data.refresh_token)

        const expiresInMilliseconds = (response.data.expires_in - 300) * 1000
        setTimeout(async () => {
          await this.renewToken()
        }, expiresInMilliseconds)

        return true
      }

      return false
    } catch (error) {
      console.error('Token renewal failed:', error)

      return false
    }
  }

  public login(): Promise<void> {
    return this.userManager.signinRedirect()
  }

  public logout(cancellationUrl: string): Promise<void> {
    return this.userManager.signoutRedirect({ extraQueryParams: { cancel_url: cancellationUrl } })
  }

  public getAccessToken(): Promise<string> {
    return this.userManager.getUser().then((data: any) => {
      return data.access_token
    })
  }
}
