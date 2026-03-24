import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext';

const RegisterForm = () => {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: '',
    mobile: '',
    email: '',
    password: '',
  });
  const [error, setError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((current) => ({ ...current, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setIsSubmitting(true);

    try {
      await register(formData);
      navigate('/commuter', { replace: true });
    } catch (err) {
      setError(typeof err === 'string' ? err : 'Registration failed.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700" htmlFor="fullName">
          Full name
        </label>
        <input
          id="fullName"
          name="name"
          type="text"
          placeholder="Charlie Johnson"
          value={formData.name}
          onChange={handleChange}
          className="w-full rounded-2xl border border-slate-200 px-4 py-3 outline-none transition focus:border-sky-500"
          required
        />
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700" htmlFor="mobile">
          Mobile number
        </label>
        <input
          id="mobile"
          name="mobile"
          type="tel"
          placeholder="9876543210"
          value={formData.mobile}
          onChange={handleChange}
          className="w-full rounded-2xl border border-slate-200 px-4 py-3 outline-none transition focus:border-sky-500"
          required
        />
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700" htmlFor="registerEmail">
          Email
        </label>
        <input
          id="registerEmail"
          name="email"
          type="email"
          placeholder="charlie@example.com"
          value={formData.email}
          onChange={handleChange}
          className="w-full rounded-2xl border border-slate-200 px-4 py-3 outline-none transition focus:border-sky-500"
          required
        />
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700" htmlFor="registerPassword">
          Password
        </label>
        <input
          id="registerPassword"
          name="password"
          type="password"
          placeholder="Create a password"
          value={formData.password}
          onChange={handleChange}
          className="w-full rounded-2xl border border-slate-200 px-4 py-3 outline-none transition focus:border-sky-500"
          required
        />
      </div>

      {error ? (
        <p className="rounded-2xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {error}
        </p>
      ) : null}

      <button
        type="submit"
        disabled={isSubmitting}
        className="w-full rounded-2xl bg-amber-400 px-4 py-3 font-semibold text-slate-950 transition hover:bg-amber-300 disabled:cursor-not-allowed disabled:opacity-70"
      >
        {isSubmitting ? 'Creating account...' : 'Create account'}
      </button>

      <p className="text-sm text-slate-600">
        Already registered?{' '}
        <Link className="font-semibold text-sky-700 hover:text-sky-800" to="/login">
          Sign in here
        </Link>
      </p>
    </form>
  );
};

export default RegisterForm;
