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
  Leaderboard: {
    getEntriesForToday: (): string => `/api/leaderboard/scores/today`,
    getEntriesForDate: (): string => `/api/leaderboard/scores/date`,
    getTopEntriesForGame: (gameId: string, topN?: number): string => {
      let endpoint = `/api/leaderboard/game/${gameId}`;
      if (topN !== undefined) {
        endpoint += `/${topN}`;
      }
      return endpoint;
    },
  },
};

export default Endpoints;
