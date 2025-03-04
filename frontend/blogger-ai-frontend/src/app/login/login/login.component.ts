import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup; 

  constructor() {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  // Metoda do obsługi formularza
  onSubmit() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value); // Wyświetlenie danych formularza
    } else {
      console.log('Formularz jest niepoprawny');
    }
  }
}
