import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { AppbarComponent } from './components/appbar/appbar.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateService } from '@ngx-translate/core';



@NgModule({
  declarations: [
    DashboardComponent,
    AppbarComponent,
    NavbarComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    TranslateModule
  ]
})
export class DashboardModule {
    constructor(private translate: TranslateService, private http: HttpClient) {
      //TODO: add support for english and other languages too
      const lang = 'pl';
      this.translate.use(lang);
      this.loadModuleTranslations('dashboard', lang);
    }
  
    private loadModuleTranslations(moduleName: string, language: string): void {
      const path = `../../assets/i18n/${moduleName}/${language}.json`;
      
      this.http.get(path).subscribe((translations: any) => {
        this.translate.setTranslation(language, translations, true);
      }, error => {
        console.error('Error loading translations:', error);
      });
    }
 }
