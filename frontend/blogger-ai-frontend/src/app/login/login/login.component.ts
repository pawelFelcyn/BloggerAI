import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup;
  isBusy = false;
  error: string | undefined;

  constructor(private httpClient: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(3)])
    });
  }
  onSubmit() {
    if (this.loginForm.invalid || this.isBusy) {
      return;
    }

    this.isBusy = true;
    this.error = undefined;

    this.httpClient.post<string>('/api/authentication/login', this.loginForm.value, {
      responseType: 'text' as 'json'
    })
    .pipe(
      finalize(() => this.isBusy = false)
    )
    .subscribe({
      next: response => {
        sessionStorage.setItem('jwt', response);
        this.router.navigate(['/dashboard']);
      },
      error: error => {
        if (error.status === 401) {
          this.error = 'INVALID_CREDENTIALS';
        } else {
          this.error = 'SOMETHING_WENT_WRONG';
        }
      }
    });
  }
}
