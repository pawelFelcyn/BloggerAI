import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
} from '@angular/common/http';
import { ConfigService } from '../services/config.service';

@Injectable({
  providedIn: 'root',
})
export class BaseUrlInterceptor implements HttpInterceptor {
  constructor(private configService: ConfigService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    if (!req.url.endsWith('.js') && !req.url.includes('/assets/')) {
      const modifiedReq = req.clone({
        url: `${this.configService.apiUrl}${req.url}`,
      });
      return next.handle(modifiedReq);
    }
    return next.handle(req);
  }
}
