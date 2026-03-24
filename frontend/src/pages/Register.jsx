import { Link } from 'react-router-dom';
import RegisterForm from '../features/auth/components/RegisterForm';

const Register = () => {
  return (
    <div className="min-h-screen bg-amber-50 px-4 py-10">
      <div className="mx-auto grid max-w-6xl gap-8 lg:grid-cols-[0.95fr_1.05fr]">
        <section className="rounded-3xl bg-white p-8 shadow-xl order-2 lg:order-1">
          <div className="mb-6">
            <h2 className="text-3xl font-bold text-slate-900">Register</h2>
            <p className="mt-2 text-sm text-slate-600">
              Create your commuter account to purchase and manage passes.
            </p>
          </div>

          <RegisterForm />

          <p className="mt-6 text-sm text-slate-600">
            Already have an account?{' '}
            <Link className="font-semibold text-sky-700 hover:text-sky-800" to="/login">
              Go to login
            </Link>
          </p>
        </section>

        <section className="order-1 rounded-3xl bg-amber-400 p-8 text-slate-950 shadow-2xl lg:order-2 lg:p-12">
          <p className="mb-4 inline-flex rounded-full bg-slate-950/10 px-4 py-1 text-sm font-medium">
            Quick signup
          </p>
          <h1 className="max-w-lg text-4xl font-black leading-tight">
            Start your city travel account in a minute.
          </h1>
          <p className="mt-5 max-w-xl text-base leading-7 text-slate-900/80">
            Register to buy passes, track usage, and validate journeys across transit modes.
          </p>
        </section>
      </div>
    </div>
  );
};

export default Register;
