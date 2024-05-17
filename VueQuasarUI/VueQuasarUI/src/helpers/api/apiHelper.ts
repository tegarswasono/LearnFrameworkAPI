import { Notify, Loading } from 'quasar'
import router from '@/router/index'
import { inject } from 'vue'
import AuthService from '../authService'
import axios, { type AxiosResponse, AxiosError } from 'axios'

export default class apiHelper {
  private authService: AuthService

  constructor() {
    this.authService = AuthService.getInstance()
  }

  public async callApi(
    endpoint: string,
    method: string = 'POST',
    body: object | null = null
  ): Promise<any> {
    try {
      Loading.show()
      const postUrl = (<any>window).appSettings.api.base_url + endpoint
      const config: Record<string, any> = {
        method: method,
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('access_token')}`
        }
      }

      if (method === 'POST' || method === 'PUT') {
        ;(<any>config).data = body
      }
      const response: AxiosResponse = await axios(postUrl, config)
      Loading.hide()

      if (response.status >= 200 && response.status < 300) {
        return response.data
      } else {
        this.handleErrorResponse(response)
      }
    } catch (err: any) {
      Loading.hide()
      this.handleErrorResponse(err)
    }
  }

  public async callApiFormData(endpoint: string, body: FormData): Promise<any> {
    try {
      Loading.show()

      const postUrl = (<any>window).appSettings.api.base_url + endpoint
      const config = {
        method: 'POST',
        headers: {
          //Authorization: `Bearer ${localStorage.getItem('access_token')}`
        },
        data: body
      }

      const response: AxiosResponse = await axios(postUrl, config)

      Loading.hide()

      if (response.status >= 200 && response.status < 300) {
        return response.data
      } else {
        this.handleErrorResponse(response)
      }
    } catch (err) {
      Loading.hide()
      throw err
    }
  }

  private async handleErrorResponse(response: AxiosResponse | AxiosError): Promise<void> {
    if (axios.isAxiosError(response)) {
      const axiosError: AxiosError = response
      if (axiosError.response) {
        const errorResponse: AxiosResponse = axiosError.response
        switch (errorResponse.status) {
          case 401:
            if ((await this.authService.renewToken()) === true) {
              window.location.reload()
            } else {
              localStorage.clear()
              Notify.create({
                type: 'negative',
                position: 'bottom-right',
                message: 'Session is expired, please re-login.',
                actions: [
                  {
                    icon: 'close',
                    color: 'white',
                    rounded: true
                  }
                ]
              })
              router.push({ name: 'loginindex' })
            }
            break
          case 403:
            Notify.create({
              type: 'negative',
              position: 'bottom-right',
              message: 'You are not eligible to access this API endpoint.',
              actions: [
                {
                  icon: 'close',
                  color: 'white',
                  rounded: true
                }
              ]
            })
            break
          case 400:
            // eslint-disable-next-line no-case-declarations
            let result = ''
            // eslint-disable-next-line no-prototype-builtins
            if (errorResponse.data.hasOwnProperty('messages')) {
              const errorMessage = errorResponse.data.messages
              result = errorMessage.join('\n')
            } else {
              result = errorResponse.data.message
            }

            Notify.create({
              type: 'negative',
              position: 'bottom-right',
              message: result as string | undefined,
              actions: [
                {
                  icon: 'close',
                  color: 'white',
                  rounded: true
                }
              ]
            })
            break
          default:
            Notify.create({
              type: 'negative',
              position: 'bottom-right',
              message: 'An unexpected error occurred, please try again.',
              actions: [
                {
                  icon: 'close',
                  color: 'white',
                  rounded: true
                }
              ]
            })
            break
        }
      } else {
        // Handle other Axios errors (e.g., network error)
      }
    } else {
      // Handle other non-Axios errors here
    }
  }
}
