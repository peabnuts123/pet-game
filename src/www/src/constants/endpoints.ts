const Endpoints = {
  TakingTree: {
    getAll: (): string => '/api/taking-tree',
    donateItem: (): string => '/api/taking-tree/donate',
    claimItem: (): string => '/api/taking-tree/claim',
  },
  Auth: {
    login: (returnUrl?: string): string => '/api/auth/login' + (returnUrl ? `?returnUrl=${encodeURIComponent(returnUrl)}` : ''),
    logout: (): string => '/api/auth/logout',
  },
  User: {
    getProfile: (): string => '/api/user/profile',
    updateProfile: (): string => '/api/user/profile',
  },
};

export default Endpoints;
