const Endpoints = {
  TakingTree: {
    getAll: (): string => '/api/taking-tree',
    donateItem: (): string => '/api/taking-tree/donate',
    claimItem: (): string => '/api/taking-tree/claim',
  },
  Auth: {
    getProfile: (): string => '/api/auth/profile',
    login: (returnUrl?: string): string => '/api/auth/login' + (returnUrl ? `?returnUrl=${encodeURIComponent(returnUrl)}` : ''),
    logout: (): string => '/api/auth/logout',
  },
};

export default Endpoints;
