// Nossa TV - Landing Page JavaScript
// ============================================

// ============================================
// Smooth Scroll
// ============================================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const href = this.getAttribute('href');
        if (href !== '#' && href !== '') {
            e.preventDefault();
            const target = document.querySelector(href);
            if (target) {
                const headerOffset = 80;
                const elementPosition = target.getBoundingClientRect().top;
                const offsetPosition = elementPosition + window.pageYOffset - headerOffset;

                window.scrollTo({
                    top: offsetPosition,
                    behavior: 'smooth'
                });

                // Close mobile menu if open
                const navMenu = document.getElementById('navMenu');
                if (navMenu && navMenu.classList.contains('active')) {
                    navMenu.classList.remove('active');
                }
            }
        }
    });
});

// ============================================
// Header Scroll Effect
// ============================================
let lastScroll = 0;
const header = document.querySelector('.main-header');

window.addEventListener('scroll', () => {
    const currentScroll = window.pageYOffset;

    if (currentScroll > 100) {
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }

    lastScroll = currentScroll;
});

// ============================================
// Mobile Menu Toggle
// ============================================
function toggleMobileMenu() {
    const navMenu = document.getElementById('navMenu');
    const toggle = document.querySelector('.mobile-menu-toggle');
    
    if (navMenu) {
        navMenu.classList.toggle('active');
        toggle.classList.toggle('active');
    }
}

// Close mobile menu when clicking outside
document.addEventListener('click', (e) => {
    const navMenu = document.getElementById('navMenu');
    const toggle = document.querySelector('.mobile-menu-toggle');
    
    if (navMenu && !navMenu.contains(e.target) && !toggle.contains(e.target)) {
        navMenu.classList.remove('active');
    }
});

// ============================================
// Categories Carousel
// ============================================
function scrollCarousel(direction) {
    const carousel = document.getElementById('categoriesCarousel');
    if (carousel) {
        const scrollAmount = 320; // card width + gap
        carousel.scrollBy({
            left: direction * scrollAmount,
            behavior: 'smooth'
        });
    }
}

// Auto-scroll categories carousel
let categoriesAutoScroll;
const startCategoriesAutoScroll = () => {
    const carousel = document.getElementById('categoriesCarousel');
    if (!carousel) return;

    categoriesAutoScroll = setInterval(() => {
        const maxScroll = carousel.scrollWidth - carousel.clientWidth;
        if (carousel.scrollLeft >= maxScroll - 10) {
            carousel.scrollTo({ left: 0, behavior: 'smooth' });
        } else {
            carousel.scrollBy({ left: 320, behavior: 'smooth' });
        }
    }, 3000);
};

// Stop auto-scroll on hover
const carousel = document.getElementById('categoriesCarousel');
if (carousel) {
    carousel.addEventListener('mouseenter', () => {
        clearInterval(categoriesAutoScroll);
    });

    carousel.addEventListener('mouseleave', () => {
        startCategoriesAutoScroll();
    });

    // Start auto-scroll
    startCategoriesAutoScroll();
}

// ============================================
// Testimonials Carousel
// ============================================
function scrollTestimonials(direction) {
    const carousel = document.getElementById('testimonialsCarousel');
    if (carousel) {
        const scrollAmount = 440; // card width + gap
        carousel.scrollBy({
            left: direction * scrollAmount,
            behavior: 'smooth'
        });
    }
}

// Auto-scroll testimonials
let testimonialsAutoScroll;
const startTestimonialsAutoScroll = () => {
    const carousel = document.getElementById('testimonialsCarousel');
    if (!carousel) return;

    testimonialsAutoScroll = setInterval(() => {
        const maxScroll = carousel.scrollWidth - carousel.clientWidth;
        if (carousel.scrollLeft >= maxScroll - 10) {
            carousel.scrollTo({ left: 0, behavior: 'smooth' });
        } else {
            carousel.scrollBy({ left: 440, behavior: 'smooth' });
        }
    }, 4000);
};

const testimonialsCarousel = document.getElementById('testimonialsCarousel');
if (testimonialsCarousel) {
    testimonialsCarousel.addEventListener('mouseenter', () => {
        clearInterval(testimonialsAutoScroll);
    });

    testimonialsCarousel.addEventListener('mouseleave', () => {
        startTestimonialsAutoScroll();
    });

    startTestimonialsAutoScroll();
}

// ============================================
// FAQ Accordion
// ============================================
function toggleFaq(button) {
    const faqItem = button.closest('.faq-item');
    const allItems = document.querySelectorAll('.faq-item');

    // Close all other items
    allItems.forEach(item => {
        if (item !== faqItem) {
            item.classList.remove('active');
        }
    });

    // Toggle current item
    faqItem.classList.toggle('active');
}

// ============================================
// Intersection Observer for Animations
// ============================================
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe all cards and sections
document.addEventListener('DOMContentLoaded', () => {
    const animatedElements = document.querySelectorAll(
        '.feature-card, .category-card, .plan-card, .testimonial-card, .faq-item'
    );

    animatedElements.forEach((el, index) => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(30px)';
        el.style.transition = `opacity 0.6s ease ${index * 0.1}s, transform 0.6s ease ${index * 0.1}s`;
        observer.observe(el);
    });
});

// ============================================
// Particles Animation
// ============================================
function createParticles() {
    const canvas = document.getElementById('particles-canvas');
    if (!canvas) return;

    const particles = [];
    const particleCount = 50;

    for (let i = 0; i < particleCount; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particle.style.cssText = `
            position: absolute;
            width: ${Math.random() * 4 + 1}px;
            height: ${Math.random() * 4 + 1}px;
            background: rgba(255, 255, 255, ${Math.random() * 0.5 + 0.2});
            border-radius: 50%;
            left: ${Math.random() * 100}%;
            top: ${Math.random() * 100}%;
            animation: particleFloat ${Math.random() * 10 + 10}s linear infinite;
            animation-delay: ${Math.random() * 5}s;
        `;
        canvas.appendChild(particle);
    }
}

// Initialize particles
createParticles();

// ============================================
// Form Validation (if needed)
// ============================================
const forms = document.querySelectorAll('form');
forms.forEach(form => {
    form.addEventListener('submit', (e) => {
        const inputs = form.querySelectorAll('input[required], textarea[required]');
        let isValid = true;

        inputs.forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                input.classList.add('error');
            } else {
                input.classList.remove('error');
            }
        });

        if (!isValid) {
            e.preventDefault();
            alert('Por favor, preencha todos os campos obrigatórios.');
        }
    });
});

// ============================================
// Lazy Loading Images
// ============================================
if ('loading' in HTMLImageElement.prototype) {
    const images = document.querySelectorAll('img[loading="lazy"]');
    images.forEach(img => {
        img.src = img.dataset.src;
    });
} else {
    // Fallback for browsers that don't support lazy loading
    const script = document.createElement('script');
    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/lazysizes/5.3.2/lazysizes.min.js';
    document.body.appendChild(script);
}

// ============================================
// Price Toggle (if implementing price switch)
// ============================================
function togglePricing(isAnnual) {
    const prices = {
        monthly: { amount: '29', period: '/mês' },
        annual: { amount: '249', period: '/ano' }
    };

    const priceElements = document.querySelectorAll('.plan-price');
    priceElements.forEach(priceEl => {
        const amountEl = priceEl.querySelector('.amount');
        const periodEl = priceEl.querySelector('.period');
        
        if (amountEl && periodEl) {
            const price = isAnnual ? prices.annual : prices.monthly;
            amountEl.textContent = price.amount;
            periodEl.textContent = price.period;
        }
    });
}

// ============================================
// Analytics / Tracking (Example)
// ============================================
function trackEvent(category, action, label) {
    // Google Analytics tracking
    if (typeof gtag !== 'undefined') {
        gtag('event', action, {
            'event_category': category,
            'event_label': label
        });
    }
    
    // Facebook Pixel tracking
    if (typeof fbq !== 'undefined') {
        fbq('track', action, { category, label });
    }
}

// Track button clicks
document.querySelectorAll('.btn-primary, .btn-secondary, .btn-nav').forEach(btn => {
    btn.addEventListener('click', () => {
        const buttonText = btn.textContent.trim();
        trackEvent('Button', 'Click', buttonText);
    });
});

// Track WhatsApp button
const whatsappBtn = document.querySelector('.whatsapp-float');
if (whatsappBtn) {
    whatsappBtn.addEventListener('click', () => {
        trackEvent('WhatsApp', 'Click', 'Float Button');
    });
}

// ============================================
// Performance Optimization
// ============================================
// Debounce function for scroll events
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// ============================================
// Console Welcome Message
// ============================================
console.log('%c?? Olá, Desenvolvedor!', 'font-size: 20px; font-weight: bold; color: #6366f1;');
console.log('%cInteressado em trabalhar conosco? Entre em contato!', 'font-size: 14px; color: #64748b;');
console.log('%c?? Nossa TV - Tecnologia e Entretenimento', 'font-size: 12px; color: #10b981;');

// ============================================
// Prevent Right Click on Images (Optional)
// ============================================
/*
document.addEventListener('contextmenu', (e) => {
    if (e.target.tagName === 'IMG') {
        e.preventDefault();
        return false;
    }
});
*/

// ============================================
// Copy Protection (Optional)
// ============================================
/*
document.addEventListener('copy', (e) => {
    e.preventDefault();
    return false;
});
*/

// ============================================
// Page Load Performance
// ============================================
window.addEventListener('load', () => {
    // Hide loading spinner if present
    const loader = document.querySelector('.page-loader');
    if (loader) {
        loader.style.opacity = '0';
        setTimeout(() => {
            loader.style.display = 'none';
        }, 300);
    }

    // Log performance metrics
    if (window.performance) {
        const perfData = window.performance.timing;
        const pageLoadTime = perfData.loadEventEnd - perfData.navigationStart;
        console.log(`? Página carregada em ${pageLoadTime}ms`);
    }
});

// ============================================
// Service Worker Registration (PWA)
// ============================================
if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register('/sw.js')
            .then(registration => {
                console.log('? Service Worker registrado:', registration);
            })
            .catch(error => {
                console.log('? Erro ao registrar Service Worker:', error);
            });
    });
}

// ============================================
// Export functions for external use
// ============================================
window.NossaTV = {
    toggleMobileMenu,
    scrollCarousel,
    scrollTestimonials,
    toggleFaq,
    togglePricing,
    trackEvent
};
