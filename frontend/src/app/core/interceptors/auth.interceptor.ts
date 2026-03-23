import { HttpHandlerFn, HttpRequest } from '@angular/common/http';

export function authInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn) {
  //temp...
  // const token = localStorage.getItem('token');
  const token = 1;

  console.log("aaaaaaaaaaaaaaaaaaa");

  if (token) {
    const reqWithHeader = request.clone({
      setHeaders: {
        Authorization: 'Bearer ' + localStorage.getItem('token'),
      },
    });

    return next(reqWithHeader);
  }

  return next(request);
}
