import { UserManager, User, WebStorageStateStore } from 'oidc-client-ts'

const config = {
  authority: 'https://localhost:5001',

  client_id: 'corporate-portal-bff',

  redirect_uri: `${window.location.origin}/callback`,
  
  post_logout_redirect_uri: `${window.location.origin}/logout-callback`,

  response_type: 'code',

  scope: 'openid profile email api',

  automaticSilentRenew: true,
  filterProtocolClaims: true,
  loadUserInfo: true,

  userStore: new WebStorageStateStore({
    store: window.localStorage
  })
}

class AuthService {
  private userManager: UserManager
  private user: User | null = null

  constructor() {
    this.userManager = new UserManager(config)
    this.loadUser()
  }

  private async loadUser() {
    try {
      this.user = await this.userManager.getUser()
    } catch (error) {
      console.error('Error loading user:', error)
      this.user = null
    }
  }

  async login(): Promise<void> {
    await this.userManager.signinRedirect()
  }

  async handleCallback(): Promise<User | null> {
    try {
      this.user = await this.userManager.signinRedirectCallback()
      return this.user
    } catch (error) {
      console.error('Signin callback error:', error)
      return null
    }
  }

  async logout(): Promise<void> {
    const user = await this.userManager.getUser()

    await this.userManager.signoutRedirect({
      id_token_hint: user?.id_token,
      post_logout_redirect_uri: config.post_logout_redirect_uri
    })
  }

  async handleLogoutCallback(): Promise<void> {
    try {
      await this.userManager.signoutRedirectCallback()
      await this.userManager.removeUser()
      this.user = null
    } catch (error) {
      console.error('Logout callback error:', error)
    }
  }

  async getAccessToken(): Promise<string | null> {
    const user = await this.userManager.getUser()

    if (user && !user.expired) {
      return user.access_token
    }

    if (user) {
      await this.userManager.signinSilent()
      const refreshedUser = await this.userManager.getUser()
      return refreshedUser?.access_token || null
    }

    return null
  }

  async isAuthenticated(): Promise<boolean> {
    const user = await this.userManager.getUser()
    return user !== null && !user.expired
  }

  async getUser(): Promise<User | null> {
    this.user = await this.userManager.getUser()
    return this.user
  }

  getUserClaims() {
    if (!this.user) return null

    return {
      sub: this.user.profile.sub,
      name: this.user.profile.name,
      email: this.user.profile.email,
      firstName: this.user.profile.first_name,
      lastName: this.user.profile.last_name,
      fullName: this.user.profile.full_name,
      photo: this.user.profile.photo
    }
  }

  onUserLoaded(callback: (user: User) => void) {
    this.userManager.events.addUserLoaded(callback)
  }

  onUserUnloaded(callback: () => void) {
    this.userManager.events.addUserUnloaded(callback)
  }

  onAccessTokenExpiring(callback: () => void) {
    this.userManager.events.addAccessTokenExpiring(callback)
  }

  onAccessTokenExpired(callback: () => void) {
    this.userManager.events.addAccessTokenExpired(callback)
  }
}

export const authService = new AuthService()
export default authService