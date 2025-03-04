import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login/login.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule,
    LoginRoutingModule,
    TranslateModule,
    ReactiveFormsModule
  ]
})
export class LoginModule {
  constructor(private translate: TranslateService, private http: HttpClient) {
    //TODO: add support for english and other languages too
    const lang = 'pl';
    this.translate.use(lang);
    this.loadModuleTranslations('login', lang);
  }

  private loadModuleTranslations(moduleName: string, language: string): void {
    const path = `../../assets/i18n/${moduleName}/${language}.json`;
    
    // Use HttpClient to fetch the JSON file
    this.http.get(path).subscribe((translations: any) => {
      this.translate.setTranslation(language, translations, true);
    }, error => {
      console.error('Error loading translations:', error);
    });
  }
 }
