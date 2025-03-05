import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = () => {
  const router = inject(Router);
  const token = sessionStorage.getItem('jwt');

  if (token && !isTokenExpired(token)) {
    return true;
  }

  sessionStorage.removeItem('jwt');
  return router.parseUrl('/login');
};

const isTokenExpired = (token: string): boolean => {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expiration = payload.exp * 1000;
    return Date.now() > expiration;
  } catch (error) {
    return true;
  }
};
