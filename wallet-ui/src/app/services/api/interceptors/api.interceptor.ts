import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { AuthTokenService } from '../auth-token.service';

@Injectable({
  providedIn: 'root'
})
export class ApiInterceptor implements HttpInterceptor {
  constructor(private authToken: AuthTokenService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authToken.token;
    if (token) {
      req = req.clone({
        url: environment.api + req.url,
        setHeaders: {
          Authorization: `Bearer ${this.authToken.token}`
        }
      });
    } else {
      req = req.clone({
        url: environment.api + req.url
      });
    }

    return next.handle(req).pipe(
      map(event => {
        if (event instanceof HttpResponse) {
          const hasAuth = event.headers.has('Authorization');
          if (hasAuth) {
            const value = event.headers.get('Authorization');
            this.authToken.token = value.split(' ')[1];
          }
          return event;
        }
      })
    );
  }
}
