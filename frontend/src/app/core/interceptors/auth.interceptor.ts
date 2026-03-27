import { HttpHandlerFn, HttpRequest } from '@angular/common/http';

export function authInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn) {

  const token = localStorage.getItem('token');

  if (token) {
    const reqWithHeader = request.clone({
      setHeaders: {
        Authorization: 'Bearer ' + token,
      },
    });

    return next(reqWithHeader);
  }

  return next(request);
}
