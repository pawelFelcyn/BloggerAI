import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {
  private config: any;

  constructor(private http: HttpClient) {}

  loadConfig() {
    return firstValueFrom(this.http.get('/assets/config/config.json')).then(
      (config) => {
        this.config = config;
      }
    );
  }

  get apiUrl(): string {
    return this.config?.apiUrl;
  }
}
