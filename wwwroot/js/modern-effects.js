// ===== MODERN DENTAL CLINIC UI EFFECTS =====

document.addEventListener('DOMContentLoaded', function() {
    
    // Initialize all modern effects
    initScrollReveal();
    initParallaxEffect();
    initSmoothScrolling();
    initCardHoverEffects();
    initLoadingAnimations();
    initTooltips();
    initCounterAnimations();
    initTypewriterEffect();
    
    console.log('🦷 Modern Dental Clinic UI Effects Loaded');
});

// ===== SCROLL REVEAL ANIMATION =====
function initScrollReveal() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('revealed');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe all elements with scroll-reveal class
    document.querySelectorAll('.scroll-reveal').forEach(el => {
        observer.observe(el);
    });

    // Auto-add scroll-reveal to cards and statistics
    document.querySelectorAll('.card, .statistics-card').forEach((el, index) => {
        el.classList.add('scroll-reveal');
        el.style.animationDelay = `${index * 0.1}s`;
        observer.observe(el);
    });
}

// ===== PARALLAX EFFECT =====
function initParallaxEffect() {
    window.addEventListener('scroll', () => {
        const scrolled = window.pageYOffset;
        const parallaxElements = document.querySelectorAll('.parallax');
        
        parallaxElements.forEach(element => {
            const speed = element.dataset.speed || 0.5;
            const yPos = -(scrolled * speed);
            element.style.transform = `translateY(${yPos}px)`;
        });
    });
}

// ===== SMOOTH SCROLLING =====
function initSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// ===== CARD HOVER EFFECTS =====
function initCardHoverEffects() {
    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-8px) scale(1.02)';
            this.style.boxShadow = '0 20px 40px rgba(46, 139, 139, 0.2)';
        });

        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
            this.style.boxShadow = '0 4px 12px rgba(46, 139, 139, 0.12)';
        });
    });

    // Statistics cards special effects
    document.querySelectorAll('.border-left-primary, .border-left-success, .border-left-info, .border-left-warning, .border-left-danger').forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-12px) scale(1.05)';
            this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        });

        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
}

// ===== LOADING ANIMATIONS =====
function initLoadingAnimations() {
    // Add loading shimmer to tables while they load
    const tables = document.querySelectorAll('table');
    tables.forEach(table => {
        if (table.querySelector('tbody').children.length === 0) {
            table.classList.add('loading-shimmer');
        }
    });

    // Remove loading states after content loads
    setTimeout(() => {
        document.querySelectorAll('.loading-shimmer').forEach(el => {
            el.classList.remove('loading-shimmer');
        });
    }, 1000);
}

// ===== TOOLTIPS =====
function initTooltips() {
    // Initialize Bootstrap tooltips if available
    if (typeof bootstrap !== 'undefined') {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Custom tooltip for buttons
    document.querySelectorAll('.btn').forEach(btn => {
        if (!btn.hasAttribute('title')) return;
        
        btn.addEventListener('mouseenter', function(e) {
            const tooltip = document.createElement('div');
            tooltip.className = 'custom-tooltip';
            tooltip.textContent = this.getAttribute('title');
            tooltip.style.cssText = `
                position: absolute;
                background: rgba(0,0,0,0.8);
                color: white;
                padding: 8px 12px;
                border-radius: 6px;
                font-size: 12px;
                z-index: 1000;
                pointer-events: none;
                opacity: 0;
                transition: opacity 0.3s ease;
            `;
            
            document.body.appendChild(tooltip);
            
            const rect = this.getBoundingClientRect();
            tooltip.style.left = rect.left + (rect.width / 2) - (tooltip.offsetWidth / 2) + 'px';
            tooltip.style.top = rect.top - tooltip.offsetHeight - 8 + 'px';
            
            setTimeout(() => tooltip.style.opacity = '1', 10);
            
            this.addEventListener('mouseleave', () => {
                tooltip.remove();
            }, { once: true });
        });
    });
}

// ===== COUNTER ANIMATIONS =====
function initCounterAnimations() {
    const counters = document.querySelectorAll('.h5, .statistics-number');
    
    const animateCounter = (counter) => {
        const target = parseInt(counter.textContent.replace(/[^\d]/g, ''));
        if (isNaN(target)) return;
        
        const duration = 2000;
        const step = target / (duration / 16);
        let current = 0;
        
        const timer = setInterval(() => {
            current += step;
            if (current >= target) {
                current = target;
                clearInterval(timer);
            }
            counter.textContent = Math.floor(current).toLocaleString();
        }, 16);
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                animateCounter(entry.target);
                observer.unobserve(entry.target);
            }
        });
    });

    counters.forEach(counter => {
        observer.observe(counter);
    });
}

// ===== TYPEWRITER EFFECT =====
function initTypewriterEffect() {
    const typewriterElements = document.querySelectorAll('.typewriter');
    
    typewriterElements.forEach(element => {
        const text = element.textContent;
        element.textContent = '';
        element.style.borderRight = '2px solid #2E8B8B';
        
        let i = 0;
        const timer = setInterval(() => {
            element.textContent += text.charAt(i);
            i++;
            if (i >= text.length) {
                clearInterval(timer);
                // Blinking cursor effect
                setInterval(() => {
                    element.style.borderRight = element.style.borderRight === '2px solid transparent' 
                        ? '2px solid #2E8B8B' 
                        : '2px solid transparent';
                }, 500);
            }
        }, 100);
    });
}

// ===== UTILITY FUNCTIONS =====

// Add stagger animation to elements
function addStaggerAnimation(selector, delay = 100) {
    document.querySelectorAll(selector).forEach((el, index) => {
        el.style.animationDelay = `${index * delay}ms`;
        el.classList.add('animate-fade-in-up');
    });
}

// Add floating animation to elements
function addFloatingAnimation(selector) {
    document.querySelectorAll(selector).forEach(el => {
        el.classList.add('floating');
    });
}

// Smooth page transitions
function initPageTransitions() {
    // Add fade-in effect to main content
    const main = document.querySelector('main');
    if (main) {
        main.classList.add('animate-fade-in-up');
    }
}

// Initialize page transitions
initPageTransitions();

// ===== RESPONSIVE UTILITIES =====
function handleResponsiveAnimations() {
    const isMobile = window.innerWidth <= 768;
    
    if (isMobile) {
        // Reduce animations on mobile for better performance
        document.querySelectorAll('.card').forEach(card => {
            card.style.transition = 'transform 0.2s ease';
        });
    }
}

window.addEventListener('resize', handleResponsiveAnimations);
handleResponsiveAnimations();

// ===== ACCESSIBILITY =====
// Respect user's motion preferences
if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
    document.documentElement.style.setProperty('--animation-duration', '0.01ms');
    document.documentElement.style.setProperty('--transition-duration', '0.01ms');
}
